using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;
using Oracle.DataAccess.Client;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// CodeService에 대한 요약 설명입니다.
	/// </summary>
	public class CodeBiz : BaseBiz
	{
		public CodeBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  코드구분목록조회
		/// </summary>
		/// <param name="codeModel"></param>
		public void GetSectionList(HeaderModel header, CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() Start");
				_log.Debug("-----------------------------------------");
				
                
				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + codeModel.Section + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT	STM_COD_CLS AS Section      ");
                sbQuery.AppendLine("    	,	STM_COD		AS Code         ");
                sbQuery.AppendLine("    	,	STM_COD_NM	AS CodeName     ");
                sbQuery.AppendLine("    FROM STM_COD                        ");
                sbQuery.AppendLine("    WHERE STM_COD = '00'              ");

				// 코드분류가 선택했으면
				if(codeModel.SearchSection.Length > 0 && !codeModel.SearchSection.Equals("00"))
				{
                    sbQuery.Append("  AND STM_COD_CLS = '" + codeModel.SearchSection + "' \n");
				}

                sbQuery.Append(" ORDER BY STM_COD_CLS \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				codeModel.CodeDataSet = ds.Copy();
				// 결과
				codeModel.ResultCnt = Utility.GetDatasetCount(codeModel.CodeDataSet);
				// 결과코드 셋트
				codeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + codeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				codeModel.ResultCD = "3000";
				codeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();

		}
	

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="codeModel"></param>
		public void GetCodeList(HeaderModel header, CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCodeList() Start");
				_log.Debug("-----------------------------------------");
				
                
				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + codeModel.Section + "]");
				// __DEBUG__

				string Code = ""; 

				StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT	STM_COD_CLS AS Section      ");
                sbQuery.AppendLine("    	,	STM_COD		AS Code         ");
                sbQuery.AppendLine("    	,	STM_COD_NM	AS CodeName     ");
                sbQuery.AppendLine("    FROM STM_COD                        ");
                sbQuery.AppendLine("    WHERE STM_COD <> '00' AND STM_COD <> '0' AND STM_COD IS NOT NULL  ");

				// 코드분류가 선택했으면
				if(codeModel.Section.Length > 0 && !codeModel.Section.Equals("00"))
				{
                    sbQuery.Append("  AND STM_COD_CLS = '" + codeModel.Section + "' \n");
				}			
				if (codeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND STM_COD_NM    = '" + codeModel.SearchKey.Trim() + "' \n");
				}

                sbQuery.Append(" ORDER BY STM_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
                    Code = Utility.GetDatasetString(ds, 0, "Code");
				}

				// 결과 DataSet의 모델에 복사
				codeModel.CodeDataSet = ds.Copy();
				// 결과
				codeModel.ResultCnt = Utility.GetDatasetCount(codeModel.CodeDataSet);
				// 결과코드 셋트
				codeModel.ResultCD = "0000";

				codeModel.Code = Code.ToString();  // 현재 ROW의 코드값을 찾기 위해

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + codeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				codeModel.ResultCD = "3000";
				codeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();

		}

		/// <summary>
		/// 코드구분 저장
		/// </summary>		
		/// <returns></returns>
		public void SetSectionUpdate(HeaderModel header, CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSectionUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    UPDATE	STM_COD                         ");
                sbQuery.AppendLine("    	SET STM_COD_CLS	= :Section          ");
                sbQuery.AppendLine("            ,STM_COD_NM	= :CodeName         ");
                sbQuery.AppendLine("    WHERE	STM_COD_CLS	= :Section_old      ");
                sbQuery.AppendLine("    	AND STM_COD     = '00'              ");

				i = 0;
                sqlParams[i++] = new OracleParameter(":Section"     , OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":CodeName"    , OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":Section_old" , OracleDbType.Char, 2);
		
				i = 0;
				sqlParams[i++].Value = codeModel.Section;						
				sqlParams[i++].Value = codeModel.CodeName;						
				sqlParams[i++].Value = codeModel.Section_old;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// __DEBUG__				
				_log.Debug("CodeName:[" + codeModel.CodeName + "]");
				_log.Debug("Section:[" + codeModel.Section + "]");
				_log.Debug("Code_old:[" + codeModel.Section_old + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("코드구분정보수정:["+codeModel.Section + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				codeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSectionUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				codeModel.ResultCD   = "3201";
				codeModel.ResultDesc = "코드구분정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 코드 저장
		/// </summary>		
		/// <returns></returns>
		public void SetCodeUpdate(HeaderModel header, CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCodeUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[4];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    UPDATE	STM_COD                     ");
                sbQuery.AppendLine("    	SET STM_COD	= :Code             ");
                sbQuery.AppendLine("            ,STM_COD_NM	= :CodeName     ");
                sbQuery.AppendLine("    WHERE	STM_COD_CLS	= :Section      ");
                sbQuery.AppendLine("    	AND STM_COD     = :Code_old     ");

				i = 0;
				sqlParams[i++] = new OracleParameter("@Code", OracleDbType.Varchar2, 3);
                sqlParams[i++] = new OracleParameter("@CodeName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter("@Section" , OracleDbType.Char, 2);
				sqlParams[i++] = new OracleParameter("@Code_old", OracleDbType.Varchar2, 3);
		
				i = 0;
				sqlParams[i++].Value = codeModel.Code;
				sqlParams[i++].Value = codeModel.CodeName;
				sqlParams[i++].Value = codeModel.Section;
				sqlParams[i++].Value = codeModel.Code_old;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// __DEBUG__
				_log.Debug("Code:[" + codeModel.Code + "]");
				_log.Debug("CodeName:[" + codeModel.CodeName + "]");
				_log.Debug("Section:[" + codeModel.Section + "]");
				_log.Debug("Code_old:[" + codeModel.Code_old + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("코드정보수정:["+codeModel.Code + "] 등록자:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				codeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCodeUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				codeModel.ResultCD   = "3201";
				codeModel.ResultDesc = "코드정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 코드 생성
		/// </summary>		
		/// <returns></returns>
		public void SetCodeCreate(HeaderModel header, CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCodeCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    INSERT INTO STM_COD ( STM_COD_CLS, STM_COD, STM_COD_NM )    ");
                sbQuery.AppendLine("    VALUES	( :Section   , :Code  , :CodeName  )                ");

                sqlParams[i++] = new OracleParameter(":Section" , OracleDbType.Char, 2);
				sqlParams[i++] = new OracleParameter(":Code", OracleDbType.Varchar2, 3);
                sqlParams[i++] = new OracleParameter(":CodeName", OracleDbType.Varchar2, 40);
								
				i = 0;
				sqlParams[i++].Value = codeModel.Section;
				sqlParams[i++].Value = codeModel.Code;
				sqlParams[i++].Value = codeModel.CodeName;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("코드정보생성:[" + codeModel.Code + "(" + codeModel.CodeName + ")] 등록자:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				codeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCodeCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				codeModel.ResultCD   = "3101";
				codeModel.ResultDesc = "코드정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 코드 삭제
		/// </summary>		
		/// <returns></returns>
		public void SetCodeDelete(HeaderModel header, CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCodeDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

                sbQuery.AppendLine("    DELETE STM_COD WHERE STM_COD_CLS  = :Section AND STM_COD  = :Code_old   ");

                sqlParams[i++] = new OracleParameter(":Section" , OracleDbType.Char, 2);
				sqlParams[i++] = new OracleParameter(":Code_old", OracleDbType.Varchar2, 3);
												
				i = 0;
				sqlParams[i++].Value = codeModel.Section;
				sqlParams[i++].Value = codeModel.Code_old;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("코드정보삭제:[" + codeModel.Code + "(" + codeModel.Code + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				codeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCodeDelete() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				codeModel.ResultCD   = "3101";
				codeModel.ResultDesc = "코드정보 삭제 중 오류가 발생하였습니다";
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