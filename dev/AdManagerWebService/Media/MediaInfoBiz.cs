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
	/// UserInfoBiz에 대한 요약 설명입니다.
	/// </summary>
	public class MediaInfoBiz : BaseBiz
	{
		public MediaInfoBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// 매체목록조회
		/// </summary>
		/// <param name="mediasModel"></param>
		public void GetUsersList(HeaderModel header, MediaInfoModel mediasModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + mediasModel.SearchKey       + "]");
				_log.Debug("SearchUserLevel:[" + mediasModel.SearchUserLevel + "]");
				// __DEBUG__			

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT MDA_COD AS MediaCode ,MDA_NM AS MediaName \n"
                    + "       ,DUTYR AS Charger ,PHONE_NO AS Tell        \n"
                    + "       ,EMAIL AS Email                \n"
                    + "       ,USE_YN AS UseYn                \n"
                    + "       ,DECODE(USE_YN,'Y','','N','사용안함') AS UseYn_N  \n"
                    + "       ,DT_INSERT AS RegDt			    \n"
                    + "		  ,DT_UPDATE AS ModDt               \n"					
					+ "  FROM MDA                 \n"			
					+ " WHERE 1 = 1                 \n"
					);

				// 어드민과 슈퍼유저 가 아니변 사용여부가 'Y' 데이터만 조회한다.
//				if (!(header.UserClass.Equals("10") || header.UserClass.Equals("20")))
//				{
//					sbQuery.Append(" AND UseYn = 'Y' \n");
//				}
				
				// 검색어가 있으면
				if (mediasModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(""
                        + "   AND ( MDA_COD   LIKE '%" + mediasModel.SearchKey.Trim() + "%' \n"
                        + "      OR MDA_NM   LIKE '%" + mediasModel.SearchKey.Trim() + "%' \n"
                        + "      OR DUTYR     LIKE '%" + mediasModel.SearchKey.Trim() + "%' \n"
                        + "      OR PHONE_NO        LIKE '%" + mediasModel.SearchKey.Trim() + "%' \n"
                        + "      OR EMAIL       LIKE '%" + mediasModel.SearchKey.Trim() + "%' \n"						
						+ "   ) \n"
						);
				}

				if(mediasModel.SearchchkAdState_10.Trim().Length > 0 && mediasModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND USE_YN = 'Y' OR USE_YN = 'N' \n");
				}	
				if(mediasModel.SearchchkAdState_10.Trim().Length > 0 && mediasModel.SearchchkAdState_10.Trim().Equals("N"))
				{
                    sbQuery.Append(" AND  USE_YN  = 'Y' \n");					
				}	

				// 매체레벨을 선택했으면
				/*if(mediasModel.SearchUserLevel.Trim().Length > 0 && !mediasModel.SearchUserLevel.Trim().Equals("0"))
				{
					sbQuery.Append(" AND UserLevel = '" + mediasModel.SearchUserLevel.Trim() + "' \n");
				}*/

                sbQuery.Append(" ORDER BY MDA_COD DESC \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체모델에 복사
				mediasModel.UserDataSet = ds.Copy();
				// 결과
				mediasModel.ResultCnt = Utility.GetDatasetCount(mediasModel.UserDataSet);
				// 결과코드 셋트
				mediasModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediasModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediasModel.ResultCD = "3000";
				mediasModel.ResultDesc = "매체정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		/// <summary>
		/// 매체정보 저장
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
		public void SetUserUpdate(HeaderModel header, MediaInfoModel mediasModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[6];

				sbQuery.Append(""
					+ "UPDATE MDA                     \n"
                    + "   SET MDA_NM  = :MediaName      \n"
					+ "      ,DUTYR    = :Charger  \n" 
					+ "      ,PHONE_NO       = :Tell     \n"
					+ "      ,EMAIL      = :Email      \n"					
					+ "      ,USE_YN      = :UseYn      \n"					
					+ "      ,DT_UPDATE  = SYSDATE      \n"
                    + " WHERE MDA_COD    = :MediaCode        \n"
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":MediaName", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":Charger", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Email", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
		

				i = 0;
				sqlParams[i++].Value = mediasModel.MediaName;
				sqlParams[i++].Value = mediasModel.Charger;
				sqlParams[i++].Value = mediasModel.Tell;
				sqlParams[i++].Value = mediasModel.Email;								
				sqlParams[i++].Value = mediasModel.UseYn;								
				sqlParams[i++].Value = Convert.ToInt32(mediasModel.MediaCode);

				_log.Debug("MediaCode:[" + mediasModel.MediaName + "]");
				_log.Debug("MediaCode:[" + mediasModel.Charger + "]");
				_log.Debug("MediaCode:[" + mediasModel.Tell + "]");
				_log.Debug("MediaCode:[" + mediasModel.Email + "]");
				_log.Debug("MediaCode:[" + mediasModel.UseYn + "]");
				
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체정보수정:["+mediasModel.MediaCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediasModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				mediasModel.ResultCD   = "3201";
				mediasModel.ResultDesc = "매체정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 매체 생성
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
		public void SetUserCreate(HeaderModel header, MediaInfoModel mediasModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[4];

				sbQuery.Append( ""
					+ "INSERT INTO MDA (                         \n"
					+ "       MDA_COD ,MDA_NM ,DUTYR ,PHONE_NO \n"
					+ "      ,EMAIL \n"
					+ "      ,USE_YN \n"
					+ "      ,DT_INSERT, DT_UPDATE         \n"					
					+ "      )                                          \n"
					+ " SELECT                                         \n"
                    + "       NVL(MAX(MDA_COD),0)+1        \n"
					+ "      ,:MediaName      \n"
					+ "      ,:Charger  \n" 
					+ "      ,:Tell     \n"
					+ "      ,:Email      \n"
					+ "      ,'Y'      \n"
					+ "      ,SYSDATE      \n"
					+ "      ,SYSDATE      \n"					
					+ " FROM MDA		\n"
					
					);


                sqlParams[i++] = new OracleParameter(":MediaName", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":Charger", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Email", OracleDbType.Varchar2, 40);
				
				i = 0;				
				sqlParams[i++].Value = mediasModel.MediaName;
				sqlParams[i++].Value = mediasModel.Charger;
				sqlParams[i++].Value = mediasModel.Tell;
				sqlParams[i++].Value = mediasModel.Email;								

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체정보생성:[" + mediasModel.MediaCode + "(" + mediasModel.MediaName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediasModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediasModel.ResultCD   = "3101";
				mediasModel.ResultDesc = "매체정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		public void SetUserDelete(HeaderModel header, MediaInfoModel mediasModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];

				sbQuery.Append(""
					+ "DELETE MDA         \n"
					+ " WHERE MDA_COD  = :MediaCode  \n"
					);

                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = mediasModel.MediaCode;

				_log.Debug("MediaCode:[" + mediasModel.MediaCode + "]");
				
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체정보삭제:[" + mediasModel.MediaCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediasModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediasModel.ResultCD   = "3301";
				mediasModel.ResultDesc = "매체정보 삭제중 오류가 발생하였습니다";
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
