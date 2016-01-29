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

namespace AdManagerWebService.Common
{
	/// <summary>
	/// CodeService에 대한 요약 설명입니다.
	/// </summary>
	public class SystemMenuBiz : BaseBiz
	{
		public SystemMenuBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  메뉴콤보목록조회
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void GetComboList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetComboList() Start");
				_log.Debug("-----------------------------------------");
                
				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MenuCode :[" + systemMenuModel.MenuCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT	MENU_COD	AS MenuCode     ");
	            sbQuery.AppendLine("        ,	MENU_NM	    AS MenuName     ");          
                sbQuery.AppendLine("    FROM STM_MENU                       ");
                sbQuery.AppendLine("    WHERE	MENU_LVL = '1'              ");
	            sbQuery.AppendLine("        AND MENU_ORD = '0'              ");
				sbQuery.AppendLine("    ORDER BY MENU_COD                   ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				systemMenuModel.SystemMenuDataSet = ds.Copy();
				// 결과
				systemMenuModel.ResultCnt = Utility.GetDatasetCount(systemMenuModel.SystemMenuDataSet);
				// 결과메뉴 셋트
				systemMenuModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + systemMenuModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetComboList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD = "3000";
				systemMenuModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();

		}

		/// <summary>
		///  1차 메뉴 목록조회
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void GetUpperMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUpperMenuList() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");
				_log.Debug("MenuCode :[" + systemMenuModel.MenuCode + "]");

				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("     SELECT	MENU_COD	as MenuCode	        ");
	            sbQuery.AppendLine("         ,	MENU_LVL	as MenuLevel        ");
	            sbQuery.AppendLine("         ,	UPPER_MENU	as UpperMenu        ");
	            sbQuery.AppendLine("         ,	MENU_NM		as MenuName	        ");
	            sbQuery.AppendLine("         ,	MENU_ORD	as MenuOrder        ");
	            sbQuery.AppendLine("         ,	USE_YN		as UseYn	        ");
                sbQuery.AppendLine("     FROM	STM_MENU				        ");
                sbQuery.AppendLine("     WHERE	MENU_LVL = '1'			        ");
	            sbQuery.AppendLine("         AND	MENU_ORD = '0'			    ");
                sbQuery.AppendLine("     ORDER BY MENU_COD                      ");
				
				_log.Debug(sbQuery.ToString());
							
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				systemMenuModel.SystemMenuDataSet = ds.Copy();
				systemMenuModel.ResultCnt = Utility.GetDatasetCount(systemMenuModel.SystemMenuDataSet);
				systemMenuModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + systemMenuModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUpperMenuList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD = "3000";
				systemMenuModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();

		}
	

		/// <summary>
		///  메뉴목록조회
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void GetMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() Start");
				_log.Debug("-----------------------------------------");
                
				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MenuCode :[" + systemMenuModel.MenuCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				// 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT	MENU_COD	as MenuCode	                                                    ");
	            sbQuery.AppendLine("        ,	MENU_LVL	as MenuLevel                                                    ");
	            sbQuery.AppendLine("        ,	UPPER_MENU	as UpperMenu                                                    ");
	            sbQuery.AppendLine("        ,	MENU_NM		as MenuName	                                                    ");
	            sbQuery.AppendLine("        , ( SELECT MENU_NM FROM STM_MENU WHERE MENU_COD = :MenuCode ) as UpperName      ");
	            sbQuery.AppendLine("        ,	MENU_ORD	as MenuOrder                                                    ");
	            sbQuery.AppendLine("        ,	USE_YN		as UseYn	                                                    ");
                sbQuery.AppendLine("    FROM	STM_MENU			                                                        ");
                sbQuery.AppendLine("    WHERE	MENU_LVL = '2'                                                              ");

				// 메뉴그룹을 선택했으면
				if(systemMenuModel.UpperMenu.Length > 0 && !systemMenuModel.UpperMenu.Equals("00"))
				{
					sbQuery.AppendLine("    AND UPPER_MENU = '" + systemMenuModel.UpperMenu + "'");
				}			
				sbQuery.AppendLine("    ORDER BY MENU_ORD");

				sqlParams[0] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 6);
				sqlParams[0].Value = systemMenuModel.MenuCode;	

				_log.Debug(sbQuery.ToString());
	
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 모델에 복사
				systemMenuModel.SystemMenuDataSet = ds.Copy();

				string LastOrder = "1";
				sbQuery   = new StringBuilder();

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT NVL(MAX(MENU_ORD),1) AS LastOrder                ");
                sbQuery.AppendLine("    FROM STM_MENU							                ");
                sbQuery.AppendLine("    WHERE UPPER_MENU = " + systemMenuModel.UpperMenu + "    ");

				_log.Debug(sbQuery.ToString());

				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
				}
				systemMenuModel.LastOrder = LastOrder;
				ds.Dispose();
				// 결과
				systemMenuModel.ResultCnt = Utility.GetDatasetCount(systemMenuModel.SystemMenuDataSet);
				systemMenuModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + systemMenuModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD = "3000";
				systemMenuModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				// 데이트베이스를  Close한다
				_db.Close();
			}
			_db.Close();
		}

		/// <summary>
		/// 메뉴구분 저장
		/// </summary>		
		/// <returns></returns>
		public void SetUpperMenuUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    UPDATE STM_MENU                 ");
	            sbQuery.AppendLine("        SET MENU_NM = :MenuName     ");
	            sbQuery.AppendLine("        ,	USE_YN  = :UseYn        ");
                sbQuery.AppendLine("    WHERE MENU_COD = :MenuCode      ");
                sbQuery.AppendLine("    AND MENU_LVL = '1'              ");

				i = 0;
                sqlParams[i++] = new OracleParameter(":MenuName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
		
				i = 0;
				sqlParams[i++].Value = systemMenuModel.MenuName;
				sqlParams[i++].Value = systemMenuModel.UseYn;
				sqlParams[i++].Value = systemMenuModel.MenuCode;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
			
				// __DEBUG__				
				_log.Debug("MenuName:[" + systemMenuModel.MenuName + "]");
				_log.Debug("UseYn:[" + systemMenuModel.UseYn + "]");
				_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴구분정보수정:["+systemMenuModel.MenuCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3201";
				systemMenuModel.ResultDesc = "메뉴구분정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 메뉴 저장
		/// </summary>		
		/// <returns></returns>
		public void SetMenuCodeUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    UPDATE STM_MENU                 ");
                sbQuery.AppendLine("        SET MENU_NM = :MenuName     ");
                sbQuery.AppendLine("        ,	USE_YN  = :UseYn        ");
                sbQuery.AppendLine("    WHERE MENU_COD = :MenuCode      ");
                sbQuery.AppendLine("    AND MENU_LVL = '2'              ");

				i = 0;
                sqlParams[i++] = new OracleParameter(":MenuName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
		
				i = 0;
				sqlParams[i++].Value = systemMenuModel.MenuName;
				sqlParams[i++].Value = systemMenuModel.UseYn;
				sqlParams[i++].Value = systemMenuModel.MenuCode;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// __DEBUG__				
				_log.Debug("MenuName:[" + systemMenuModel.MenuName + "]");
				_log.Debug("UseYn:[" + systemMenuModel.UseYn + "]");
				_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴정보수정:["+systemMenuModel.MenuCode + "] 등록자:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeUpdate() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3201";
				systemMenuModel.ResultDesc = "메뉴정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 메뉴 생성
		/// </summary>		
		/// <returns></returns>
		public void SetUpperMenuCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    INSERT INTO STM_MENU ( MENU_COD,	MENU_LVL,	UPPER_MENU,	MENU_NM,	MENU_ORD,	USE_YN )    ");              
			    sbQuery.AppendLine("                VALUES	 (:MenuCode,	'1',		:MenuCode_2,:MenuName,	'0',		'Y')        ");

				i = 0;
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":MenuCode_2", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":MenuName", OracleDbType.Varchar2, 40);
						
				i = 0;
				sqlParams[i++].Value = systemMenuModel.MenuCode;
				sqlParams[i++].Value = systemMenuModel.MenuCode_2;
				sqlParams[i++].Value = systemMenuModel.MenuName;
				
				// __DEBUG__				
				_log.Debug("MenuName:[" + systemMenuModel.MenuName + "]");
				_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
				_log.Debug("MenuCode_2:[" + systemMenuModel.MenuCode_2 + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__	
								
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴정보생성:[" + systemMenuModel.MenuCode + "(" + systemMenuModel.MenuCode + ")] 등록자:[" + header.UserID + "]");
					systemMenuModel.MenuCode = systemMenuModel.MenuCode;  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "메뉴정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 메뉴 생성
		/// </summary>		
		/// <returns></returns>
		public void SetMenuCodeCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[4];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    INSERT INTO STM_MENU (                                                                              ");
		        sbQuery.AppendLine("             MENU_COD                                                                                   ");
		        sbQuery.AppendLine("            ,MENU_LVL                                                                                   ");
		        sbQuery.AppendLine("            ,UPPER_MENU                                                                                 ");
		        sbQuery.AppendLine("            ,MENU_NM                                                                                    ");
		        sbQuery.AppendLine("            ,MENU_ORD                                                                                   ");
		        sbQuery.AppendLine("            ,USE_YN )                                                                                   ");
	            sbQuery.AppendLine("        SELECT	:MenuCode                                                                               ");
		        sbQuery.AppendLine("            ,	'2'                                                                                     ");
		        sbQuery.AppendLine("            ,	MAX((SELECT UPPER_MENU                                                                  ");
				sbQuery.AppendLine("                    FROM STM_MENU                                                                       ");
                sbQuery.AppendLine("                    WHERE ROWNUM <= 1                                                                   ");
                sbQuery.AppendLine("                    AND UPPER_MENU LIKE '%'|| SUBSTR(TO_CHAR(:MenuCode_2),0,3) || '%')) AS UpperMenu_1  ");
		        sbQuery.AppendLine("            ,	:MenuName                                                                               ");
		        sbQuery.AppendLine("            ,	NVL(MAX(MENU_ORD), 0) + 1                                                               ");
		        sbQuery.AppendLine("            ,	'Y'				                                                                        ");
	            sbQuery.AppendLine("        FROM STM_MENU                                                                                   ");
	            sbQuery.AppendLine("        WHERE UPPER_MENU = :UpperMenu                                                                   ");

				i = 0;
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":MenuCode_2", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":MenuName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":UpperMenu", OracleDbType.Char, 6);						

				i = 0;
				sqlParams[i++].Value = systemMenuModel.MenuCode;							
				sqlParams[i++].Value = systemMenuModel.MenuCode_2;							
				sqlParams[i++].Value = systemMenuModel.MenuName;	
				sqlParams[i++].Value = systemMenuModel.UpperMenu;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// __DEBUG__				
				_log.Debug("MenuCode1:[" + systemMenuModel.MenuCode + "]");
				_log.Debug("MenuName:[" + systemMenuModel.MenuName + "]");				
				_log.Debug("MenuCode2:[" + systemMenuModel.MenuCode_2 + "]");
				_log.Debug("UpperMenu:[" + systemMenuModel.UpperMenu + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__	
								
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴정보생성:[" + systemMenuModel.MenuCode + "(" + systemMenuModel.MenuCode + ")] 등록자:[" + header.UserID + "]");
					systemMenuModel.MenuCode = systemMenuModel.MenuCode;  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "메뉴정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 메뉴 삭제
		/// </summary>		
		/// <returns></returns>
		public void SetUpperMenuDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQuery2 = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    DELETE STM_POWER                                                                ");
                sbQuery.AppendLine("    WHERE MENU_COD IN (SELECT MENU_COD FROM STM_MENU WHERE UPPER_MENU = :UpperMenu) ");

                sbQuery2.AppendLine("");
                sbQuery2.AppendLine("    DELETE STM_MENU                 ");
                sbQuery2.AppendLine("    WHERE UPPER_MENU = :UpperMenu   ");

				try
				{
					_db.BeginTran();
                    sqlParams[i++] = new OracleParameter(":UpperMenu", OracleDbType.Char, 6);
																	
					i = 0;
					sqlParams[i++].Value = systemMenuModel.UpperMenu;

                    i = 0;
                    sqlParams[i++].Direction = ParameterDirection.Input;

					_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
				
					_log.Debug(sbQuery.ToString());
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sqlParams = new OracleParameter[1];
						
					i = 0;
                    sqlParams[i++] = new OracleParameter(":UpperMenu", OracleDbType.Char, 6);		
					
					i = 0;
					sqlParams[i++].Value = systemMenuModel.UpperMenu;

                    i = 0;
                    sqlParams[i++].Direction = ParameterDirection.Input;
					
					rc =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
													
					// 쿼리실행					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴정보삭제:[" + systemMenuModel.UpperMenu + "(" + systemMenuModel.UpperMenu + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "메뉴정보 삭제 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		/// <summary>
		/// 메뉴 삭제
		/// </summary>		
		/// <returns></returns>
		public void SetMenuCodeDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQuery2 = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    DELETE STM_POWER WHERE MENU_COD = :MenuCode     ");

                sbQuery2.AppendLine("");
                sbQuery2.AppendLine("    DELETE STM_MENU WHERE MENU_COD = :MenuCode     ");

				try
				{				
					_db.BeginTran();
                    sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
																	
					i = 0;
					sqlParams[i++].Value = systemMenuModel.MenuCode;

                    i = 0;
                    sqlParams[i++].Direction = ParameterDirection.Input;

					_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
				
					_log.Debug(sbQuery.ToString());
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sqlParams = new OracleParameter[1];
						
					i = 0;
                    sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);		
					
					i = 0;
					sqlParams[i++].Value = systemMenuModel.MenuCode;

                    i = 0;
                    sqlParams[i++].Direction = ParameterDirection.Input;

					_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
				
					_log.Debug(sbQuery2.ToString());
					
					rc =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
												
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴정보삭제:[" + systemMenuModel.MenuCode + "(" + systemMenuModel.MenuCode + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "메뉴정보 삭제 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		#region 메뉴 순위올림
		/// <summary>
		/// 메뉴  순위올림
		/// </summary>
		/// <returns></returns>
		public void SetMenuCodeOrderUp(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderUp() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");				
				_log.Debug("MenuOrder :[" + systemMenuModel.MenuOrder	   + "]");		// 메뉴 순위				
				// __DEBUG__
				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    SELECT NVL(MAX(MENU_ORD),1) AS UpOrder              ");
                    sbQuery.AppendLine("    FROM STM_MENU                                       ");
                    sbQuery.AppendLine("    WHERE MENU_ORD < " + systemMenuModel.MenuOrder       );

					// __DEBUG__				
					_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
					_log.Debug("MenuOrder:[" + systemMenuModel.MenuOrder + "]");				
									
					_log.Debug(sbQuery.ToString());
					// __DEBUG__	

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "UpOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// 해당 순위를 0순위로 임시변경
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                     ");
	                sbQuery.AppendLine("        SET MENU_ORD = 0                                ");
                    sbQuery.AppendLine("    WHERE	MENU_COD = " + systemMenuModel.MenuCode      );
	                sbQuery.AppendLine("        AND MENU_ORD = " + systemMenuModel.MenuOrder     );

					// __DEBUG__				
					_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
					_log.Debug("MenuOrder:[" + systemMenuModel.MenuOrder + "]");
									
					_log.Debug(sbQuery.ToString());
					// __DEBUG__	

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// 삭제후 해당 순위보다 변경할 순위의 내역을 해당 순위로 조정
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                                                ");
	                sbQuery.AppendLine("        SET MENU_ORD = " + systemMenuModel.MenuOrder                                );
                    sbQuery.AppendLine("    WHERE	MENU_ORD = " + ToOrder                                                  );
	                sbQuery.AppendLine("        AND MENU_COD IN (	SELECT MENU_COD                                        ");
					sbQuery.AppendLine("    	                    FROM STM_MENU                                          ");
					sbQuery.AppendLine("    	                    WHERE UPPER_MENU = " + systemMenuModel.UpperMenu + " ) ");

					// __DEBUG__									
					_log.Debug("MenuOrder:[" + systemMenuModel.MenuOrder + "]");
					_log.Debug("UpperMenu:[" + systemMenuModel.UpperMenu + "]");
									
					_log.Debug(sbQuery.ToString());
					// __DEBUG__	

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

                    // 해당 순위로  변경
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                 ");   
	                sbQuery.AppendLine("        SET MENU_ORD = " + ToOrder                   );
                    sbQuery.AppendLine("    WHERE MENU_COD = " + systemMenuModel.MenuCode    );
	                sbQuery.AppendLine("        AND MENU_ORD = 0                            ");

                    // __DEBUG__				
					_log.Debug("MenuCode:[" + systemMenuModel.MenuCode + "]");
					_log.Debug(sbQuery.ToString());
                    // __DEBUG__	

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴 순위올림 변경:[" + systemMenuModel.MenuCode + "] 등록자:[" + header.UserID + "]");
					systemMenuModel.MenuOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = " 메뉴 순위올림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 메뉴  순위내림
		/// <summary>
		/// 메뉴  순위내림
		/// </summary>
		/// <returns></returns>
		public void SetMenuCodeOrderDown(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderDown() Start");
				_log.Debug("-----------------------------------------");

				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    SELECT NVL(MIN(MENU_ORD),1) AS DownOrder        ");
                    sbQuery.AppendLine("    FROM STM_MENU                                   ");
                    sbQuery.AppendLine("    WHERE MENU_ORD > " + systemMenuModel.MenuOrder   );

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "DownOrder");
					}

					ds.Dispose();
 		
					_db.BeginTran();

					// 해당 순위를 0순위로 임시변경
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                     ");
	                sbQuery.AppendLine("        SET MENU_ORD = 0                                ");
                    sbQuery.AppendLine("    WHERE MENU_COD = " + systemMenuModel.MenuCode        );
	                sbQuery.AppendLine("        AND MENU_ORD = " + systemMenuModel.MenuOrder     );

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// 삭제후 해당 순위보다 변경할 순위의 내역을 해당 순위로 조정
					sbQuery   = new StringBuilder();
					
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                                                     ");
	                sbQuery.AppendLine("        SET MENU_ORD = " + systemMenuModel.MenuOrder                                     );
                    sbQuery.AppendLine("    WHERE MENU_ORD = " + ToOrder                                                         );
	                sbQuery.AppendLine("        AND MENU_COD IN (	SELECT MENU_COD                                             ");
					sbQuery.AppendLine("    	                    FROM STM_MENU                                               ");
					sbQuery.AppendLine("    	                    WHERE UPPER_MENU = " + systemMenuModel.UpperMenu + " )      ");

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                         ");
	                sbQuery.AppendLine("        SET MENU_ORD = " + ToOrder                           );
                    sbQuery.AppendLine("    WHERE	MENU_COD = " + systemMenuModel.MenuCode          );
	                sbQuery.AppendLine("        AND MENU_ORD = 0                                    ");

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("메뉴 순위내림 변경:[" + systemMenuModel.MenuCode + "] 등록자:[" + header.UserID + "]");
					systemMenuModel.MenuOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = " 메뉴 순위내림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion	
	}
}
