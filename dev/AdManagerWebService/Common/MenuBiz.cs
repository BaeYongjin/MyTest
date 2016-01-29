using System;
using System.Data;
using System.Data.SqlClient;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;

using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
    /// <summary>
	/// [사용함]
    /// MenuService에 대한 요약 설명입니다.
    /// </summary>
    public class MenuBiz : BaseBiz
    {
        public MenuBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        ///  코드목록조회
        /// </summary>
        /// <param name="codeModel"></param>
        public void GetUserMenuList(HeaderModel header, MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuList() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("UserClass :[" + header.UserClass + "]");

				_db.Open();
                StringBuilder sbQuery = new StringBuilder();
				OracleParameter[] param = new OracleParameter[1];

                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT  B.MENU_COD   as MenuCode                    ");
                sbQuery.AppendLine("        ,   B.MENU_LVL   as MenuLevel                   ");
                sbQuery.AppendLine("        ,   B.MENU_NM	 as MenuName                    ");
                sbQuery.AppendLine("        ,   A.MENU_POWER as MenuPower                   ");
                sbQuery.AppendLine("    FROM  STM_POWER A                                   ");
                sbQuery.AppendLine("    LEFT JOIN STM_MENU B ON B.MENU_COD = A.MENU_COD     ");
                sbQuery.AppendLine("    WHERE	A.USER_CLS = :UserCls                       ");
	            sbQuery.AppendLine("        AND B.USE_YN = 'Y'                              ");
                sbQuery.AppendLine("    ORDER BY B.UPPER_MENU, B.MENU_ORD                   ");

                _log.Debug(sbQuery.ToString());
				                				
				param[0] = new OracleParameter(":UserID", OracleDbType.Char, 2);
				param[0].Value = header.UserClass;
				param[0].Direction = ParameterDirection.Input;

				DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(), param);

                menuModel.MenuDataSet = ds.Copy();
                menuModel.ResultCnt = Utility.GetDatasetCount(menuModel.MenuDataSet);
                menuModel.ResultCD = "0000";

                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + menuModel.ResultCnt + "]");

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserMenuList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD = "3000";
                menuModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
                _db.Close();
            }
            _db.Close();
        }

        /// <summary>
        ///  사용자 정보 목록조회
        /// </summary>
        /// <param name="codeModel"></param>
        public void GetUserClassList(HeaderModel header, MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() Start");
                _log.Debug("-----------------------------------------");

                // 데이트베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("UserClass :[" + header.UserClass + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT	STM_COD		AS Code         ");
	            sbQuery.AppendLine("        ,	STM_COD_NM	AS CodeName     ");
                sbQuery.AppendLine("    FROM STM_COD	                    ");
                sbQuery.AppendLine("    WHERE	STM_COD_CLS = '12'          ");
	            sbQuery.AppendLine("        And STM_COD <> '00'	            ");
                sbQuery.AppendLine("    ORDER BY STM_COD		            ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                menuModel.MenuDataSet = ds.Copy();
                // 결과
                menuModel.ResultCnt = Utility.GetDatasetCount(menuModel.MenuDataSet);
                // 결과코드 셋트
                menuModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + menuModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD = "3000";
                menuModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
                // 데이트베이스를  Close한다
                _db.Close();
            }
            // 데이트베이스를  Close한다
            _db.Close();
        }
        /// <summary>
        ///  메뉴권한 목록조회
        /// </summary>
        /// <param name="codeModel"></param>
        public void GetMenuPowerList(HeaderModel header, MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() Start");
                _log.Debug("-----------------------------------------");

                // 데이트베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("UserClass :[" + header.UserClass + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    SELECT	A.MENU_COD AS MenuCode                                                                          ");
	            sbQuery.AppendLine("        ,	A.MENU_NM  AS MenuName                                                                          ");
	            sbQuery.AppendLine("        ,	'"+menuModel.UserClassCode+"' UserClass		                                                    ");
	            sbQuery.AppendLine("        ,	CASE WHEN INSTR(B.MENU_POWER, 'C') > 0	                                                        ");
			    sbQuery.AppendLine("                THEN 'True' ELSE 'False' END CheckCreate                                                    ");
	            sbQuery.AppendLine("        ,	CASE WHEN INSTR(B.MENU_POWER, 'R') > 0                                                          ");
			    sbQuery.AppendLine("                THEN 'True' ELSE 'False' END CheckRead	                                                    ");
	            sbQuery.AppendLine("        ,	CASE WHEN INSTR(B.MENU_POWER, 'U') > 0                                                          ");
			    sbQuery.AppendLine("                THEN 'True' ELSE 'False' END CheckUpdate	                                                ");
	            sbQuery.AppendLine("        ,	CASE WHEN INSTR(B.MENU_POWER, 'D') > 0		                                                    ");
			    sbQuery.AppendLine("                THEN 'True' ELSE 'False' END CheckDelete		                                            ");
                sbQuery.AppendLine("    FROM STM_MENU A                                                                                         ");
                sbQuery.AppendLine("    LEFT JOIN STM_POWER B ON (B.USER_CLS = '"+menuModel.UserClassCode+"' AND A.MENU_COD = B.MENU_COD)       ");
                sbQuery.AppendLine("    WHERE A.USE_YN = 'Y'	                                                                                ");
                sbQuery.AppendLine("    ORDER BY  A.MENU_COD                                                                                    ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                menuModel.MenuDataSet = ds.Copy();
                // 결과
                menuModel.ResultCnt = Utility.GetDatasetCount(menuModel.MenuDataSet);
                // 결과코드 셋트
                menuModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + menuModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD = "3000";
                menuModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
                // 데이트베이스를  Close한다
                _db.Close();
            }
            // 데이트베이스를  Close한다
            _db.Close();
        }

        ///  <summary>
        /// 사용자 정보 생성 및 메뉴권한 인서트
        /// </summary>
        /// <returns></returns>
        public void SetUserClassCreate(HeaderModel header,MenuModel menuModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractITemHistoryInsert() Start");
                _log.Debug("-----------------------------------------");

                //코드관리 테이블에 사용자 정보 입력
                StringBuilder sbQueryUserClassInsert = new StringBuilder();
                sbQueryUserClassInsert.AppendLine("");
                sbQueryUserClassInsert.AppendLine("INSERT INTO STM_COD VALUES ('12', :UserClassCode, :UserClassName)	");

                //메뉴권한테이블에 데이터 인서트
                StringBuilder sbQueryMenuPowerInsert = new StringBuilder();
                sbQueryMenuPowerInsert.AppendLine("");
                sbQueryMenuPowerInsert.AppendLine(" INSERT INTO STM_POWER		                                ");
                sbQueryMenuPowerInsert.AppendLine("     SELECT '" + menuModel.UserClassCode + "', MENU_COD, ''	");
                sbQueryMenuPowerInsert.AppendLine("     FROM STM_MENU                			                ");

                OracleParameter[] UserClassInsertParams = new OracleParameter[2];
                                
                int i = 0;
                UserClassInsertParams[i++] = new OracleParameter(":UserClassCode", OracleDbType.Varchar2, 2);
                UserClassInsertParams[i++] = new OracleParameter(":UserClassName", OracleDbType.Varchar2, 40);

                i = 0;
                UserClassInsertParams[i++].Value = menuModel.UserClassCode;
                UserClassInsertParams[i++].Value = menuModel.UserClassName;

                i = 0;
                UserClassInsertParams[i++].Direction = ParameterDirection.Input;
                UserClassInsertParams[i++].Direction = ParameterDirection.Input;

                try
                {
                    // __DEBUG__
                    _log.Debug(sbQueryUserClassInsert.ToString());
                    // __DEBUG__
                    // __DEBUG__
                    _log.Debug(sbQueryMenuPowerInsert.ToString());
                    // __DEBUG__
                    _db.BeginTran();

                    int rc =  _db.ExecuteNonQueryParams(sbQueryUserClassInsert.ToString(), UserClassInsertParams);

                    rc =  _db.ExecuteNonQuery(sbQueryMenuPowerInsert.ToString());

                    // __MESSAGE__
                    _log.Message("메뉴권한 정보생성:["+menuModel.UserClassName + "] 등록자:[" + header.UserID + "]");
            
                    _db.CommitTran();
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                menuModel.ResultCD = "0000";  // 정상
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractCreate() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                menuModel.ResultCD   = "3101";
                menuModel.ResultDesc = "사용자 정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        // 메뉴권한정보 수정

        public void SetUserClassUpdate(HeaderModel header, MenuModel menuModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractITemHistoryInsert() Start");
                _log.Debug("-----------------------------------------");

                //코드관리 테이블에 사용자 정보 입력
                StringBuilder sbQueryUserClassUpdate = new StringBuilder();
                sbQueryUserClassUpdate.AppendLine("");
                sbQueryUserClassUpdate.AppendLine("     UPDATE STM_COD				            ");
                sbQueryUserClassUpdate.AppendLine("        SET STM_COD_NM = :UserClassName	    ");
                sbQueryUserClassUpdate.AppendLine("     WHERE	STM_COD_CLS = '12'			    ");
	            sbQueryUserClassUpdate.AppendLine("         AND STM_COD = :UserClassCode		");

                //메뉴권한테이블에 데이터 업데이트
                StringBuilder sbQueryMenuPowerUpdate = new StringBuilder();
                sbQueryMenuPowerUpdate.AppendLine("");
                sbQueryMenuPowerUpdate.AppendLine("     UPDATE STM_POWER                    ");
	            sbQueryMenuPowerUpdate.AppendLine("         SET MENU_POWER = :MenuPower     ");
                sbQueryMenuPowerUpdate.AppendLine("     WHERE	USER_CLS = :UserClass       ");
	            sbQueryMenuPowerUpdate.AppendLine("         AND MENU_COD =  :MenuCode       ");
                
                //메뉴권한테이블에 빠진데이터 인서트
                StringBuilder sbQueryMenuPowerInsert = new StringBuilder();
                sbQueryMenuPowerInsert.AppendLine("");
                sbQueryMenuPowerInsert.AppendLine("     INSERT INTO STM_POWER   ");
                sbQueryMenuPowerInsert.AppendLine("                (MENU_POWER  ");
                sbQueryMenuPowerInsert.AppendLine("                ,USER_CLS    ");
                sbQueryMenuPowerInsert.AppendLine("                ,MENU_COD )  ");
                sbQueryMenuPowerInsert.AppendLine("          VALUES             ");
                sbQueryMenuPowerInsert.AppendLine("                (:MenuPower  ");
                sbQueryMenuPowerInsert.AppendLine("                ,:UserClass  ");
                sbQueryMenuPowerInsert.AppendLine("                ,:MenuCode ) ");

                OracleParameter[] UserClassUpdateParams = new OracleParameter[2];
                                
                int i = 0;
                UserClassUpdateParams[i++] = new OracleParameter(":UserClassName", OracleDbType.Varchar2, 40);
                UserClassUpdateParams[i++] = new OracleParameter(":UserClassCode", OracleDbType.Varchar2, 2);
                
                i = 0;
                UserClassUpdateParams[i++].Value = menuModel.UserClassName;
                UserClassUpdateParams[i++].Value = menuModel.UserClassCode;

                i = 0;
                UserClassUpdateParams[i++].Direction = ParameterDirection.Input;
                UserClassUpdateParams[i++].Direction = ParameterDirection.Input;

                try
                {
                    // __DEBUG__
                    _log.Debug(sbQueryUserClassUpdate.ToString());
                    // __DEBUG__
                    // __DEBUG__
                    _log.Debug(sbQueryMenuPowerUpdate.ToString());
                    // __DEBUG__
                    _db.BeginTran();

                    int rc =  _db.ExecuteNonQueryParams(sbQueryUserClassUpdate.ToString(), UserClassUpdateParams);

                    //메뉴 권한 인서트 & 업데이트용 파라미터
                    OracleParameter[] MenuPowerParams = new OracleParameter[3];

                    //넘어온 데이터셋 전체를 한번에 업데이트 시켜주는 로직
                    for(int count=0; count < menuModel.MenuDataSet.Tables["MenuPower"].Rows.Count; count++)
                    {
                        i = 0;
                        MenuPowerParams[i++] = new OracleParameter(":MenuPower", OracleDbType.Varchar2, 4);
                        MenuPowerParams[i++] = new OracleParameter(":UserClass", OracleDbType.Char, 2);
                        MenuPowerParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
                        
                        i = 0;
                        string MenuPower = "";
                        string MenuCode = menuModel.MenuDataSet.Tables["MenuPower"].Rows[count][0].ToString();
                        string UserClassCode = menuModel.MenuDataSet.Tables["MenuPower"].Rows[count][2].ToString();
                        if(menuModel.MenuDataSet.Tables["MenuPower"].Rows[count][3].ToString().Equals("True"))
                        {
                            MenuPower += "C";
                        }
                        if(menuModel.MenuDataSet.Tables["MenuPower"].Rows[count][4].ToString().Equals("True"))
                        {   
                            MenuPower += "R";
                        }
                        if(menuModel.MenuDataSet.Tables["MenuPower"].Rows[count][5].ToString().Equals("True"))
                        {
                            MenuPower += "U";
                        }
                        if(menuModel.MenuDataSet.Tables["MenuPower"].Rows[count][6].ToString().Equals("True"))
                        {
                            MenuPower += "D";
                        }

                        _log.Debug("MenuPower=>"+ MenuPower);
                        _log.Debug("UserClassCode=>"+ UserClassCode);
                        _log.Debug("MenuCode=>"+ MenuCode);

                        MenuPowerParams[i++].Value = MenuPower;
                        MenuPowerParams[i++].Value = UserClassCode;
                        MenuPowerParams[i++].Value = MenuCode;

                        i = 0;
                        MenuPowerParams[i++].Direction = ParameterDirection.Input;
                        MenuPowerParams[i++].Direction = ParameterDirection.Input;
                        MenuPowerParams[i++].Direction = ParameterDirection.Input;

                        // __DEBUG__
                        _log.Debug(sbQueryMenuPowerUpdate.ToString());
                        // __DEBUG__

                        rc = 0;
                        rc =  _db.ExecuteNonQueryParams(sbQueryMenuPowerUpdate.ToString(), MenuPowerParams);
                        _log.Debug("Count-->"+count+"rc-->"+rc);

                        if (rc == 0)
                        {
                            // 오라클 파라미터 재사용시 Direction 재설정 필요
                            i = 0;
                            MenuPowerParams[i++] = new OracleParameter(":MenuPower", OracleDbType.Varchar2, 4);
                            MenuPowerParams[i++] = new OracleParameter(":UserClass", OracleDbType.Char, 2);
                            MenuPowerParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);

                            i = 0;
                            MenuPowerParams[i++].Value = MenuPower;
                            MenuPowerParams[i++].Value = UserClassCode;
                            MenuPowerParams[i++].Value = MenuCode;

                            i = 0;
                            MenuPowerParams[i++].Direction = ParameterDirection.Input;
                            MenuPowerParams[i++].Direction = ParameterDirection.Input;
                            MenuPowerParams[i++].Direction = ParameterDirection.Input;

                            rc = _db.ExecuteNonQueryParams(sbQueryMenuPowerInsert.ToString(), MenuPowerParams);
                        }
                    }

                    // __MESSAGE__
                    _log.Message("메뉴권한 정보 수정:["+menuModel.UserClassName + "수정자:[" + header.UserID + "]");
            
                    _db.CommitTran();
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                menuModel.ResultCD = "0000";  // 정상
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD   = "3101";
                menuModel.ResultDesc = "사용자 정보 수정 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        // 메뉴권한정보 수정

        public void SetMenuPowerUpdate(HeaderModel header, MenuModel menuModel)
        {
         
        }
    }
}
