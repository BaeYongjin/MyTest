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
	/// CodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SystemMenuBiz : BaseBiz
	{
		public SystemMenuBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  �޴��޺������ȸ
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void GetComboList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetComboList() Start");
				_log.Debug("-----------------------------------------");
                
				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MenuCode :[" + systemMenuModel.MenuCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
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
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				systemMenuModel.SystemMenuDataSet = ds.Copy();
				// ���
				systemMenuModel.ResultCnt = Utility.GetDatasetCount(systemMenuModel.SystemMenuDataSet);
				// ����޴� ��Ʈ
				systemMenuModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + systemMenuModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetComboList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD = "3000";
				systemMenuModel.ResultDesc = "�޴����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();

		}

		/// <summary>
		///  1�� �޴� �����ȸ
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void GetUpperMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUpperMenuList() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<�Է�����>");
				_log.Debug("MenuCode :[" + systemMenuModel.MenuCode + "]");

				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				// ��������
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
							
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				systemMenuModel.SystemMenuDataSet = ds.Copy();
				systemMenuModel.ResultCnt = Utility.GetDatasetCount(systemMenuModel.SystemMenuDataSet);
				systemMenuModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + systemMenuModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUpperMenuList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD = "3000";
				systemMenuModel.ResultDesc = "�޴����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();

		}
	

		/// <summary>
		///  �޴������ȸ
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void GetMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() Start");
				_log.Debug("-----------------------------------------");
                
				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MenuCode :[" + systemMenuModel.MenuCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				// ��������
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

				// �޴��׷��� ����������
				if(systemMenuModel.UpperMenu.Length > 0 && !systemMenuModel.UpperMenu.Equals("00"))
				{
					sbQuery.AppendLine("    AND UPPER_MENU = '" + systemMenuModel.UpperMenu + "'");
				}			
				sbQuery.AppendLine("    ORDER BY MENU_ORD");

				sqlParams[0] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 6);
				sqlParams[0].Value = systemMenuModel.MenuCode;	

				_log.Debug(sbQuery.ToString());
	
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �𵨿� ����
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
				// ���
				systemMenuModel.ResultCnt = Utility.GetDatasetCount(systemMenuModel.SystemMenuDataSet);
				systemMenuModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + systemMenuModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD = "3000";
				systemMenuModel.ResultDesc = "�޴����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			_db.Close();
		}

		/// <summary>
		/// �޴����� ����
		/// </summary>		
		/// <returns></returns>
		public void SetUpperMenuUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
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

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�޴�������������:["+systemMenuModel.MenuCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3201";
				systemMenuModel.ResultDesc = "�޴��������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �޴� ����
		/// </summary>		
		/// <returns></returns>
		public void SetMenuCodeUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
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

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�޴���������:["+systemMenuModel.MenuCode + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeUpdate() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3201";
				systemMenuModel.ResultDesc = "�޴����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �޴� ����
		/// </summary>		
		/// <returns></returns>
		public void SetUpperMenuCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
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
								
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�޴���������:[" + systemMenuModel.MenuCode + "(" + systemMenuModel.MenuCode + ")] �����:[" + header.UserID + "]");
					systemMenuModel.MenuCode = systemMenuModel.MenuCode;  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "�޴����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �޴� ����
		/// </summary>		
		/// <returns></returns>
		public void SetMenuCodeCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
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
								
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�޴���������:[" + systemMenuModel.MenuCode + "(" + systemMenuModel.MenuCode + ")] �����:[" + header.UserID + "]");
					systemMenuModel.MenuCode = systemMenuModel.MenuCode;  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "�޴����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �޴� ����
		/// </summary>		
		/// <returns></returns>
		public void SetUpperMenuDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
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
													
					// ��������					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�޴���������:[" + systemMenuModel.UpperMenu + "(" + systemMenuModel.UpperMenu + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUpperMenuDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "�޴����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �޴� ����
		/// </summary>		
		/// <returns></returns>
		public void SetMenuCodeDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
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
					_log.Message("�޴���������:[" + systemMenuModel.MenuCode + "(" + systemMenuModel.MenuCode + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMenuCodeDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = "�޴����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		#region �޴� �����ø�
		/// <summary>
		/// �޴�  �����ø�
		/// </summary>
		/// <returns></returns>
		public void SetMenuCodeOrderUp(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderUp() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				_log.Debug("MenuOrder :[" + systemMenuModel.MenuOrder	   + "]");		// �޴� ����				
				// __DEBUG__
				
				// ��������
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// ������ ����
					string ToOrder = "1";

					// �ش� ������ ���� ����
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

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "UpOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// �ش� ������ 0������ �ӽú���
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

					// ������ �ش� �������� ������ ������ ������ �ش� ������ ����
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

                    // �ش� ������  ����
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
					_log.Message("�޴� �����ø� ����:[" + systemMenuModel.MenuCode + "] �����:[" + header.UserID + "]");
					systemMenuModel.MenuOrder = ToOrder.ToString();  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = " �޴� �����ø� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region �޴�  ��������
		/// <summary>
		/// �޴�  ��������
		/// </summary>
		/// <returns></returns>
		public void SetMenuCodeOrderDown(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderDown() Start");
				_log.Debug("-----------------------------------------");

				// ��������
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// ������ ����
					string ToOrder = "1";

					// �ش� ������ ���� ����
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    SELECT NVL(MIN(MENU_ORD),1) AS DownOrder        ");
                    sbQuery.AppendLine("    FROM STM_MENU                                   ");
                    sbQuery.AppendLine("    WHERE MENU_ORD > " + systemMenuModel.MenuOrder   );

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "DownOrder");
					}

					ds.Dispose();
 		
					_db.BeginTran();

					// �ش� ������ 0������ �ӽú���
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                     ");
	                sbQuery.AppendLine("        SET MENU_ORD = 0                                ");
                    sbQuery.AppendLine("    WHERE MENU_COD = " + systemMenuModel.MenuCode        );
	                sbQuery.AppendLine("        AND MENU_ORD = " + systemMenuModel.MenuOrder     );

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// ������ �ش� �������� ������ ������ ������ �ش� ������ ����
					sbQuery   = new StringBuilder();
					
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                                                     ");
	                sbQuery.AppendLine("        SET MENU_ORD = " + systemMenuModel.MenuOrder                                     );
                    sbQuery.AppendLine("    WHERE MENU_ORD = " + ToOrder                                                         );
	                sbQuery.AppendLine("        AND MENU_COD IN (	SELECT MENU_COD                                             ");
					sbQuery.AppendLine("    	                    FROM STM_MENU                                               ");
					sbQuery.AppendLine("    	                    WHERE UPPER_MENU = " + systemMenuModel.UpperMenu + " )      ");

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// �ش� ������  ����
					sbQuery   = new StringBuilder();
                    
                    sbQuery.AppendLine("");
                    sbQuery.AppendLine("    UPDATE STM_MENU                                         ");
	                sbQuery.AppendLine("        SET MENU_ORD = " + ToOrder                           );
                    sbQuery.AppendLine("    WHERE	MENU_COD = " + systemMenuModel.MenuCode          );
	                sbQuery.AppendLine("        AND MENU_ORD = 0                                    ");

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�޴� �������� ����:[" + systemMenuModel.MenuCode + "] �����:[" + header.UserID + "]");
					systemMenuModel.MenuOrder = ToOrder.ToString();  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				systemMenuModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetMenuCodeOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				systemMenuModel.ResultCD   = "3101";
				systemMenuModel.ResultDesc = " �޴� �������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion	
	}
}
