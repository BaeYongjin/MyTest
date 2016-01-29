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
	/// UserInfoBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class UserInfoBiz : BaseBiz
	{
		public UserInfoBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// ����ڸ����ȸ
		/// </summary>
		/// <param name="usersModel"></param>
		public void GetUsersList(HeaderModel header, UserInfoModel usersModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + usersModel.SearchKey       + "]");
				_log.Debug("SearchUserLevel:[" + usersModel.SearchUserLevel + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + " SELECT a.USER_ID AS UserID		                \n"
                    + "	      ,a.USER_NM AS UserName	                \n"
                    + "       ,a.USER_PWD AS UserPassword	            \n"
                    + "	      ,a.USER_LVL AS UserLevel                  \n"
                    + "       ,a.USER_CLS AS UserClass                  \n"
                    + "       ,c.STM_COD_NM AS UserClassName  \n"
					+ "       ,''  AS MediaCode   \n"
                    + "       ,NVL(a.REP_COD,0)    AS RapCode     \n"
                    + "       ,NVL(a.AGNC_COD,0) AS AgencyCode  \n"
                    + "       ,b.STM_COD_NM AS UserLevelName  \n"
                    + "       ,a.USER_DEPT AS UserDept	                \n"
                    + "       ,a.USER_TITLE AS UserTitle                  \n"
                    + "       ,a.USER_PHONE AS UserTell			        \n"
                    + "       ,a.USER_MOB AS UserMobile                 \n"
                    + "       ,a.USER_EMAIL AS UserEMail                  \n"
                    + "       ,TO_CHAR(A.LAST_LOGIN, 'YYYY-MM-DD') AS LastLogin \n"
                    + "       ,a.USE_YN AS UseYn                                       \n"
                    + "       ,DECODE(A.USE_YN,'Y','','N','������') AS UseYn_N  \n"
                    + "       ,TO_CHAR(A.DT_INSERT, 'YYYY-MM-DD') AS RegDt         \n"
                    + "       ,TO_CHAR(A.DT_UPDATE, 'YYYY-MM-DD') AS ModDt         \n"
                    + "       ,a.USER_MEMO as UserComment                                 \n"
                    + "  FROM STM_USER a LEFT JOIN STM_COD b            \n"
                    + "                           ON (a.USER_LVL = b.STM_COD and b.STM_COD_CLS = '11') \n"	// 11:���ȷ���
                    + "                    LEFT JOIN STM_COD c            \n"
                    + "                           ON (a.USER_CLS = c.STM_COD and c.STM_COD_CLS = '12') \n"	// 12:����ڱ���
					+ " WHERE 1 = 1  \n"
					);


				// ������ �ƴϸ� ������� �����͸� ��ȸ�Ѵ�.
//				if (!header.UserClass.Equals("10"))
//				{
//					sbQuery.Append(" AND a.UseYn = 'Y'  AND a.UserClass <> '10' \n");
//				}
				
				// �˻�� ������
				if (usersModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
                        + "    a.USER_ID      LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
                        + " OR a.USER_NM    LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
                        + " OR a.USER_DEPT    LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
                        + " OR a.USER_TITLE   LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
                        + " OR a.USER_PHONE    LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
                        + " OR a.USER_MOB  LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
                        + " OR a.USER_MEMO LIKE '%" + usersModel.SearchKey.Trim() + "%' \n"
						+ " ) ");
				}

				// ����ڷ����� ����������
				if(usersModel.SearchUserLevel.Trim().Length > 0 && !usersModel.SearchUserLevel.Trim().Equals("00"))
				{
                    sbQuery.Append(" AND A.USER_LVL = '" + usersModel.SearchUserLevel.Trim() + "' \n");
				}			
				if(usersModel.SearchUserClass.Trim().Length > 0 && !usersModel.SearchUserClass.Trim().Equals("00"))
				{
                    sbQuery.Append(" AND A.USER_CLS = '" + usersModel.SearchUserClass.Trim() + "' \n");
				}			

				if(usersModel.SearchchkAdState_10.Trim().Length > 0 && usersModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND A.USE_YN = 'Y' OR A.USE_YN = 'N' \n");
				}	
				if(usersModel.SearchchkAdState_10.Trim().Length > 0 && usersModel.SearchchkAdState_10.Trim().Equals("N"))
				{
                    sbQuery.Append(" AND  a.USE_YN  = 'Y' \n");					
				}

                sbQuery.Append(" ORDER BY USER_CLS, USER_LVL, USER_NM \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ����ڸ𵨿� ����
				usersModel.UserDataSet = ds.Copy();
				// ���
				usersModel.ResultCnt = Utility.GetDatasetCount(usersModel.UserDataSet);
				// ����ڵ� ��Ʈ
				usersModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + usersModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				usersModel.ResultCD = "3000";
				usersModel.ResultDesc = "��������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}

		}


		/// <summary>
		/// ��������� ����
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
		public void SetUserUpdate(HeaderModel header, UserInfoModel usersModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[15];

				sbQuery.Append(""
                    + "UPDATE STM_USER                     \n"
                    + "   SET USER_NM      = :UserName      \n"
                    + "      ,USER_PWD  = :UserPassword  \n"
                    + "      ,USER_LVL     = :UserLevel     \n"
                    + "      ,USER_CLS     = :UserClass     \n"
					//+ "      ,MediaCode     = :MediaCode     \n"
                    + "      ,REP_COD       = :RapCode       \n"
                    + "      ,AGNC_COD    = :AgencyCode    \n"
                    + "      ,USER_DEPT      = :UserDept      \n"
                    + "      ,USER_TITLE     = :UserTitle     \n"
                    + "      ,USER_PHONE      = :UserTell      \n"
                    + "      ,USER_MOB    = :UserMobile    \n"
                    + "      ,USER_EMAIL     = :UserEMail     \n"
                    + "      ,USER_MEMO   = :UserComment   \n"
                    + "      ,USE_YN         = :UseYn         \n"
                    + "      ,DT_UPDATE         = SYSDATE      \n"
					+ "      ,ID_UPDATE     = :RegID         \n"
                    + " WHERE USER_ID       = :UserID        \n"
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":UserName", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":UserPassword", OracleDbType.Varchar2, 30);
                sqlParams[i++] = new OracleParameter(":UserLevel", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":UserClass", OracleDbType.Char, 2);
                //sqlParams[i++] = new OracleParameter(":MediaCode"    , SqlDbType.TinyInt);

                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":UserDept", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":UserTitle", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":UserTell", OracleDbType.Varchar2, 15);

                sqlParams[i++] = new OracleParameter(":UserMobile", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":UserEMail", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":UserComment", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);

                sqlParams[i++] = new OracleParameter(":UserID", OracleDbType.Varchar2, 10);

				i = 0;
				sqlParams[i++].Value = usersModel.UserName;
				sqlParams[i++].Value = usersModel.UserPassword;
				sqlParams[i++].Value = usersModel.UserLevel;
				sqlParams[i++].Value = usersModel.UserClass;
                /*
				if(usersModel.MediaCode.Equals("0"))	// ��ü�ڵ尡 0�̸� NULL�� ��Ʈ�Ѵ�.
				{
					sqlParams[i++].Value = null;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(usersModel.MediaCode);
				}*/
				if(usersModel.RapCode.Equals("0"))		// �̵��ڵ尡 0�̸� NULL�� ��Ʈ�Ѵ�.
				{
					sqlParams[i++].Value = null;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(usersModel.RapCode);
				}
				if(usersModel.AgencyCode.Equals("0"))	// ������ڵ尡 0�̸� NULL�� ��Ʈ�Ѵ�.
				{
					sqlParams[i++].Value = null;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(usersModel.AgencyCode);
				}
				sqlParams[i++].Value = usersModel.UserDept;
				sqlParams[i++].Value = usersModel.UserTitle;
				sqlParams[i++].Value = usersModel.UserTell;
				sqlParams[i++].Value = usersModel.UserMobile;
				sqlParams[i++].Value = usersModel.UserEMail;
				sqlParams[i++].Value = usersModel.UserComment;
				sqlParams[i++].Value = usersModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // �����
				sqlParams[i++].Value = usersModel.UserID;

				_log.Debug("UserID:[" + usersModel.UserID + "]");			

				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������������:["+usersModel.UserID + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				usersModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				usersModel.ResultCD   = "3201";
				usersModel.ResultDesc = "��������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// ����� ����
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
		public void SetUserCreate(HeaderModel header, UserInfoModel usersModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[14];

				sbQuery.Append( ""
                    + "INSERT INTO STM_USER (                         \n"
                    + "       USER_ID ,USER_NM ,USER_PWD ,USER_LVL, USER_CLS \n"
                    + "      ,REP_COD ,AGNC_COD \n"
                    + "      ,USER_DEPT ,USER_TITLE ,USER_PHONE ,USER_MOB, USER_EMAIL \n"
                    + "      ,USER_MEMO ,USE_YN , DT_INSERT, DT_UPDATE         \n"
                    + "      ,ID_INSERT                                     \n"
					+ "      )                                          \n"
					+ " VALUES (                                        \n"
					+ "       :UserID        \n"
					+ "      ,:UserName      \n"
					+ "      ,:UserPassword  \n" 
					+ "      ,:UserLevel     \n"
					+ "      ,:UserClass     \n"
					//+ "      ,:MediaCode     \n"
					+ "      ,:RapCode       \n"
					+ "      ,:AgencyCode    \n"
					+ "      ,:UserDept      \n"
					+ "      ,:UserTitle     \n"
					+ "      ,:UserTell      \n"
					+ "      ,:UserMobile    \n"
					+ "      ,:UserEMail     \n"
					+ "      ,:UserComment   \n"
					+ "      ,'Y'            \n"
					+ "      ,SYSDATE      \n"
					+ "      ,SYSDATE      \n"
					+ "      ,:RegID         \n"
					+ "      )               \n"
					);

				sqlParams[i++] = new OracleParameter(":UserID"       , OracleDbType.Varchar2 , 10);
				sqlParams[i++] = new OracleParameter(":UserName"     , OracleDbType.Varchar2 , 20);
				sqlParams[i++] = new OracleParameter(":UserPassword" , OracleDbType.Varchar2 , 30);
                sqlParams[i++] = new OracleParameter(":UserLevel", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":UserClass", OracleDbType.Char, 2);

				//sqlParams[i++] = new OracleParameter(":MediaCode"    , SqlDbType.TinyInt);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":UserDept"     , OracleDbType.Varchar2 , 20);
				sqlParams[i++] = new OracleParameter(":UserTitle"    , OracleDbType.Varchar2 , 20);

				sqlParams[i++] = new OracleParameter(":UserTell"     , OracleDbType.Varchar2 , 15);
				sqlParams[i++] = new OracleParameter(":UserMobile"   , OracleDbType.Varchar2 , 15);
				sqlParams[i++] = new OracleParameter(":UserEMail"    , OracleDbType.Varchar2 , 40);
				sqlParams[i++] = new OracleParameter(":UserComment"  , OracleDbType.Varchar2 , 50);
				sqlParams[i++] = new OracleParameter(":RegID"        , OracleDbType.Varchar2 , 10);

				i = 0;
				sqlParams[i++].Value = usersModel.UserID.Trim();	// Ȥ�� ID�� ������ �ִٸ� ���ش�. 
				sqlParams[i++].Value = usersModel.UserName;
				sqlParams[i++].Value = usersModel.UserPassword;
				sqlParams[i++].Value = usersModel.UserLevel;
				sqlParams[i++].Value = usersModel.UserClass;
                /*
				if(usersModel.MediaCode.Equals("0"))	// ��ü�ڵ尡 0�̸� NULL�� ��Ʈ�Ѵ�.
				{
					sqlParams[i++].Value = null;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(usersModel.MediaCode);
				}*/
				if(usersModel.RapCode.Equals("0"))		// �̵��ڵ尡 0�̸� NULL�� ��Ʈ�Ѵ�.
				{
					sqlParams[i++].Value = null;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(usersModel.RapCode);
				}
				if(usersModel.AgencyCode.Equals("0"))	// ������ڵ尡 0�̸� NULL�� ��Ʈ�Ѵ�.
				{
					sqlParams[i++].Value = null;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(usersModel.AgencyCode);
				}
				sqlParams[i++].Value = usersModel.UserDept;
				sqlParams[i++].Value = usersModel.UserTitle;
				sqlParams[i++].Value = usersModel.UserTell;
				sqlParams[i++].Value = usersModel.UserMobile;
				sqlParams[i++].Value = usersModel.UserEMail;
				sqlParams[i++].Value = usersModel.UserComment;
				sqlParams[i++].Value = header.UserID;				// �����

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������������:[" + usersModel.UserID + "(" + usersModel.UserName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				usersModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				usersModel.ResultCD   = "3101";
				usersModel.ResultDesc = "��������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}

		}


		public void SetUserDelete(HeaderModel header, UserInfoModel usersModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];

				sbQuery.Append(""
                    + "DELETE STM_USER         \n"
                    + " WHERE USER_ID  = :UserID  \n"
					);

                sqlParams[i++] = new OracleParameter(":UserID", OracleDbType.Varchar2, 10);

				i = 0;
				sqlParams[i++].Value = usersModel.UserID;

				_log.Debug("UserID:[" + usersModel.UserID + "]");			

				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������������:[" + usersModel.UserID + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;

				}

				usersModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				usersModel.ResultCD   = "3301";
				usersModel.ResultDesc = "��������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
	}
}
