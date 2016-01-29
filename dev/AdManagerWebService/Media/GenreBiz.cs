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
	/// GenreBiz에 대한 요약 설명입니다.
	/// </summary>
	public class GenreBiz : BaseBiz
	{
		public GenreBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// 장르목록조회
		/// </summary>
		/// <param name="genreModel"></param>
		public void GetGenreList(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + genreModel.SearchKey       + "]");
				_log.Debug("SearchGenreLevel:[" + genreModel.SearchGenreLevel + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.MediaCode, A.GenreCode, A.GenreName, A.ModDt, B.MediaName  \n"							
					+ "  FROM Genre A, Media B                \n"										
					+ "  WHERE A.MediaCode = B.MediaCode    \n"										
					);
				
				// 검색어가 있으면
				if (genreModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "    A.GenreName      LIKE '%" + genreModel.SearchKey.Trim() + "%' \n"	
						+ " OR B.MediaName    LIKE '%" + genreModel.SearchKey.Trim() + "%' \n"							
						+ " ) ");
				}

				// 장르레벨을 선택했으면
				if(genreModel.SearchGenreLevel.Trim().Length > 0 && !genreModel.SearchGenreLevel.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + genreModel.SearchGenreLevel.Trim() + "' \n");
				}			

				sbQuery.Append(" ORDER BY A.MediaCode, A.GenreCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르모델에 복사
				genreModel.UserDataSet = ds.Copy();
				// 결과
				genreModel.ResultCnt = Utility.GetDatasetCount(genreModel.UserDataSet);
				// 결과코드 셋트
				genreModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + genreModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				genreModel.ResultCD = "3000";
				genreModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		/// <summary>
		/// 장르정보 저장
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
		public void SetGenreUpdate(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];

				sbQuery.Append(""
					+ "UPDATE Genre                     \n"
					+ "   SET GenreName      = @GenreName      \n"					
					+ "      ,ModDt      = GETDATE()      \n"					
					+ " WHERE MediaCode        = @MediaCode        \n"
					+ " AND GenreCode        = @GenreCode        \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@GenreName"     , SqlDbType.VarChar , 20);				
				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GenreCode"     , SqlDbType.Int);
		

				i = 0;
				sqlParams[i++].Value = genreModel.GenreName;						
				sqlParams[i++].Value = Convert.ToInt32(genreModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(genreModel.GenreCode);

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("장르정보수정:["+genreModel.GenreCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				genreModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				genreModel.ResultCD   = "3201";
				genreModel.ResultDesc = "장르정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 장르 생성
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
		public void SetGenreCreate(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append( ""
					+ "INSERT INTO Genre (                         \n"
					+ "       MediaCode ,GenreCode ,GenreName,ModDt \n"					
					+ "      )                                          \n"
					+ " SELECT                                         \n"
					+ "      @MediaCode      \n"						
					+ "      ,ISNULL(MAX(GenreCode),0)+1        \n"
					+ "      ,@GenreName,GETDATE()      \n"						
					+ " FROM Genre		\n"					
					);

				
				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GenreName" , SqlDbType.VarChar , 20);
								
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(genreModel.MediaCode);
				sqlParams[i++].Value = genreModel.GenreName;
				
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("장르정보생성:[" + genreModel.GenreCode + "(" + genreModel.GenreName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				genreModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				genreModel.ResultCD   = "3101";
				genreModel.ResultDesc = "장르정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		public void SetGenreDelete(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE Genre         \n"
					+ " WHERE MediaCode  = @MediaCode  \n"
					+ " AND GenreCode  = @GenreCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GenreCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = genreModel.MediaCode;
				sqlParams[i++].Value = genreModel.GenreCode;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("장르정보삭제:[" + genreModel.GenreCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				genreModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				genreModel.ResultCD   = "3301";
				genreModel.ResultDesc = "장르정보 삭제중 오류가 발생하였습니다";
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

