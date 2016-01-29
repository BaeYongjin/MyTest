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
	/// [�����]
    /// MenuService�� ���� ��� �����Դϴ�.
    /// </summary>
    public class MenuBiz : BaseBiz
    {
        public MenuBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        ///  �ڵ�����ȸ
        /// </summary>
        /// <param name="codeModel"></param>
        public void GetUserMenuList(HeaderModel header, MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuList() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<�Է�����>");
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

                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + menuModel.ResultCnt + "]");

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserMenuList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD = "3000";
                menuModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
                _db.Close();
            }
            _db.Close();
        }

        /// <summary>
        ///  ����� ���� �����ȸ
        /// </summary>
        /// <param name="codeModel"></param>
        public void GetUserClassList(HeaderModel header, MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() Start");
                _log.Debug("-----------------------------------------");

                // ����Ʈ���̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("UserClass :[" + header.UserClass + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
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
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                menuModel.MenuDataSet = ds.Copy();
                // ���
                menuModel.ResultCnt = Utility.GetDatasetCount(menuModel.MenuDataSet);
                // ����ڵ� ��Ʈ
                menuModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + menuModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD = "3000";
                menuModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
                // ����Ʈ���̽���  Close�Ѵ�
                _db.Close();
            }
            // ����Ʈ���̽���  Close�Ѵ�
            _db.Close();
        }
        /// <summary>
        ///  �޴����� �����ȸ
        /// </summary>
        /// <param name="codeModel"></param>
        public void GetMenuPowerList(HeaderModel header, MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() Start");
                _log.Debug("-----------------------------------------");

                // ����Ʈ���̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("UserClass :[" + header.UserClass + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
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
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                menuModel.MenuDataSet = ds.Copy();
                // ���
                menuModel.ResultCnt = Utility.GetDatasetCount(menuModel.MenuDataSet);
                // ����ڵ� ��Ʈ
                menuModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + menuModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUserClassList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD = "3000";
                menuModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
                // ����Ʈ���̽���  Close�Ѵ�
                _db.Close();
            }
            // ����Ʈ���̽���  Close�Ѵ�
            _db.Close();
        }

        ///  <summary>
        /// ����� ���� ���� �� �޴����� �μ�Ʈ
        /// </summary>
        /// <returns></returns>
        public void SetUserClassCreate(HeaderModel header,MenuModel menuModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractITemHistoryInsert() Start");
                _log.Debug("-----------------------------------------");

                //�ڵ���� ���̺� ����� ���� �Է�
                StringBuilder sbQueryUserClassInsert = new StringBuilder();
                sbQueryUserClassInsert.AppendLine("");
                sbQueryUserClassInsert.AppendLine("INSERT INTO STM_COD VALUES ('12', :UserClassCode, :UserClassName)	");

                //�޴��������̺� ������ �μ�Ʈ
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
                    _log.Message("�޴����� ��������:["+menuModel.UserClassName + "] �����:[" + header.UserID + "]");
            
                    _db.CommitTran();
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                menuModel.ResultCD = "0000";  // ����
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractCreate() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                menuModel.ResultCD   = "3101";
                menuModel.ResultDesc = "����� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        // �޴��������� ����

        public void SetUserClassUpdate(HeaderModel header, MenuModel menuModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractITemHistoryInsert() Start");
                _log.Debug("-----------------------------------------");

                //�ڵ���� ���̺� ����� ���� �Է�
                StringBuilder sbQueryUserClassUpdate = new StringBuilder();
                sbQueryUserClassUpdate.AppendLine("");
                sbQueryUserClassUpdate.AppendLine("     UPDATE STM_COD				            ");
                sbQueryUserClassUpdate.AppendLine("        SET STM_COD_NM = :UserClassName	    ");
                sbQueryUserClassUpdate.AppendLine("     WHERE	STM_COD_CLS = '12'			    ");
	            sbQueryUserClassUpdate.AppendLine("         AND STM_COD = :UserClassCode		");

                //�޴��������̺� ������ ������Ʈ
                StringBuilder sbQueryMenuPowerUpdate = new StringBuilder();
                sbQueryMenuPowerUpdate.AppendLine("");
                sbQueryMenuPowerUpdate.AppendLine("     UPDATE STM_POWER                    ");
	            sbQueryMenuPowerUpdate.AppendLine("         SET MENU_POWER = :MenuPower     ");
                sbQueryMenuPowerUpdate.AppendLine("     WHERE	USER_CLS = :UserClass       ");
	            sbQueryMenuPowerUpdate.AppendLine("         AND MENU_COD =  :MenuCode       ");
                
                //�޴��������̺� ���������� �μ�Ʈ
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

                    //�޴� ���� �μ�Ʈ & ������Ʈ�� �Ķ����
                    OracleParameter[] MenuPowerParams = new OracleParameter[3];

                    //�Ѿ�� �����ͼ� ��ü�� �ѹ��� ������Ʈ �����ִ� ����
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
                            // ����Ŭ �Ķ���� ����� Direction �缳�� �ʿ�
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
                    _log.Message("�޴����� ���� ����:["+menuModel.UserClassName + "������:[" + header.UserID + "]");
            
                    _db.CommitTran();
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                menuModel.ResultCD = "0000";  // ����
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                menuModel.ResultCD   = "3101";
                menuModel.ResultDesc = "����� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }

        // �޴��������� ����

        public void SetMenuPowerUpdate(HeaderModel header, MenuModel menuModel)
        {
         
        }
    }
}
