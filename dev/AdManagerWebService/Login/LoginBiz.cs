// ===============================================================================
//
// LoginBiz.cs
//
// �α��� ó������ 
//
// ===============================================================================
// Release history
// 2007.08.15 RH.Jung Initialize
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
using System;
using System.Data;
using System.Data.SqlClient;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Login
{
	/// <summary>
	/// LoginService�� ���� ��� �����Դϴ�.
	/// </summary>
    public class LoginBiz : BaseBiz
	{
		public LoginBiz() : base(FrameSystem.connDbString, true)
		{
		    _log    = FrameSystem.oLog;			// �α� ��ü
		}

		public string PasswordEncrypt(string str)
		{
			return Security.Encrypt(str);			
		}

		public string PasswordDecrypt(string str)
		{
			return Security.Decrypt(str);			
		}

		/// <summary>
		/// �α��� ó��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="loginModel"></param>
		public void LoginCheck(HeaderModel header, LoginModel loginModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "LoginCheck() Start");
				_log.Debug("-----------------------------------------");
  
				_log.Debug("<�Է�����>");
				_log.Debug("UserID       :[" + loginModel.UserID       + "]");
				_log.Debug("UserPassword :[" + loginModel.UserPassword + "]");
				_log.Debug("SystemVersion:[" + loginModel.SystemVersion + "]");
				
				if(FrameSystem.m_SystemVersion == null || FrameSystem.m_SystemVersion.Equals("") || !FrameSystem.m_SystemVersion.Equals(loginModel.SystemVersion))
				{
					loginModel.ResultCD = "2000";	

					throw new FrameException("���α׷������� ������ ��ġ���� �ʽ��ϴ�.\n���α׷��� ������Ʈ�Ͻñ� �ٶ��ϴ�.\n\n��������:" + FrameSystem.m_SystemVersion);
				}

				if(loginModel.UserID.Trim().Length == 0)
				{
					loginModel.ResultCD = "2001"; // �����ID�� �Էµ��� �ʾ���.

					throw new FrameException("�����ID�� �Էµ��� �ʾҽ��ϴ�.");
				}

				_db.Open();
				StringBuilder sbQuery    = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				sbQuery.Append("\n" + "SELECT	a.USER_ID");
				sbQuery.Append("\n" + "		,	a.USER_NM		as UserName");
				sbQuery.Append("\n" + "		,	a.USER_PWD		as UserPassword");
				sbQuery.Append("\n" + "		,	a.USER_LVL		as UserLevel");
				sbQuery.Append("\n"	+ "		,	b.STM_COD_NM	AS LevelName				");
				sbQuery.Append("\n"	+ "     ,	a.USER_CLS		AS UserClass                ");
				sbQuery.Append("\n"	+ "     ,	c.STM_COD_NM	AS ClassName                ");
				sbQuery.Append("\n"	+ "     ,   1				AS MediaCode				");
				sbQuery.Append("\n"	+ "     ,	(select MDA_NM from MDA b where B.MDA_COD = 1) MediaName");
				sbQuery.Append("\n"	+ "     ,	NVL(a.REP_COD,'')	AS RapCode             ");
				sbQuery.Append("\n"	+ "     ,	(select REP_NM from MDA_REP b where a.REP_COD = b.REP_COD) RapName");
				sbQuery.Append("\n"	+ "     ,	NVL(a.AGNC_COD,'') AS AgencyCode          ");
				sbQuery.Append("\n"	+ "     ,	(select AGNC_NM from AGNC b where a.AGNC_COD = b.AGNC_COD) AgencyName");
				sbQuery.Append("\n"	+ "     ,	TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') LoginTime");
				sbQuery.Append("\n"	+ "     ,	LAST_LOGIN LastLogin");
				sbQuery.Append("\n"	+ "  FROM STM_USER a LEFT JOIN STM_COD b ON (a.USER_LVL = b.STM_COD and b.STM_COD_CLS = '11')");
				sbQuery.Append("\n" + "                  LEFT JOIN STM_COD c ON (a.USER_CLS = c.STM_COD and c.STM_COD_CLS = '12')");
				sbQuery.Append("\n" + " WHERE USER_ID = :UserID ");
				sbQuery.Append("\n" + "   AND USE_YN  = 'Y'");
			
				sqlParams[0] = new OracleParameter(":UserID", OracleDbType.Varchar2, 10);
				sqlParams[0].Value = loginModel.UserID;
				sqlParams[0].Direction = ParameterDirection.Input;
				
				_log.Debug(sbQuery.ToString());

				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
				
				if (Utility.GetDatasetCount(ds) == 0)
				{
					loginModel.ResultCD = "2002"; // �ش� ID�� DB�� �������� ����
					ds.Dispose();
					_db.Close();
					throw new FrameException("�ش�ID�� �������� �ʽ��ϴ�.");
				}

                //��й�ȣ �˾Ƴ���
                //_log.Debug("PW:"+Utility.GetDatasetString(ds, 0, "UserPassword") +">"+Security.Decrypt(Utility.GetDatasetString(ds, 0, "UserPassword")));
				// ��ȣȭ�� ��й�ȣ�� �ٸ��� ��й�ȣ ����
				if (!Utility.GetDatasetString(ds, 0, "UserPassword").Equals(loginModel.UserPassword))
				{
					loginModel.ResultCD = "2003";  // ��й�ȣ�� �ٸ�
					ds.Dispose();
					_db.Close();
					throw new FrameException("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
				}				
						
				// �α���������Ʈ
				loginModel.UserName    = Utility.GetDatasetString(ds, 0, "UserName");
				loginModel.UserLevel   = Utility.GetDatasetString(ds, 0, "UserLevel");
				loginModel.LevelName   = Utility.GetDatasetString(ds, 0, "LevelName");
				loginModel.UserClass   = Utility.GetDatasetString(ds, 0, "UserClass");
				loginModel.ClassName   = Utility.GetDatasetString(ds, 0, "ClassName");
				loginModel.MediaCode   = Utility.GetDatasetString(ds, 0, "MediaCode");
				loginModel.MediaName   = Utility.GetDatasetString(ds, 0, "MediaName");
				loginModel.RapCode     = Utility.GetDatasetString(ds, 0, "RapCode");				
				loginModel.RapName     = Utility.GetDatasetString(ds, 0, "RapName");
				loginModel.AgencyCode  = Utility.GetDatasetString(ds, 0, "AgencyCode");
				loginModel.AgencyName  = Utility.GetDatasetString(ds, 0, "AgencyName");
				loginModel.LoginTime   = Utility.GetDatasetString(ds, 0, "LoginTime");
				loginModel.LastLogin   = Utility.GetDatasetString(ds, 0, "LastLogin");

				#region ��ü������ ���� �����Ұ��� ����
				// ������� ������ ��ü������ ���
				//if(loginModel.UserLevel.Equals("20"))
				//{
					//Media ���̺��� ��ü�ڵ��� ��뿩�θ� �˾ƿ��� ����
					//1. User���̺� �α��� ID�� ��ü������ ������ �ִ� ����ڰ� �α����� �Ѵ�.
					//2. Media���̺��� ��ü�ڵ��� ��뿩�θ� Ȯ�� ��뿩�ΰ� 'N'�̸�
					//3.��ü������ �������ִ� ����ڴ� �α��� �� �� ����.
					//4.�α��� �ҷ��� ���� ������ ������ �ִ� ����� Media���̺��� �ش� ��ü�ڵ��� ��뿩�θ� 'Y'�� ���� ����� �Ѵ�.
					//sbQuery    = new StringBuilder();
					//sbQuery.Append("\n"
					//    + " SELECT *             \n"						
					//    + "   FROM Media         \n"					
					//    + "  WHERE UseYn = 'Y'   \n"
					//    + "    AND MediaCode = '" + loginModel.MediaCode + "'  \n"
					//    );

					////__DEBUG
					//_log.Debug(sbQuery.ToString());
					////__DEBUG

					//ds = new DataSet();
					//_db.ExecuteQuery(ds, sbQuery.ToString());

					////��ü ����Ʈ�� �Ǽ��� 0���� ũ�� ���� 
					//if(Utility.GetDatasetCount(ds) == 0)
					//{
					//        loginModel.ResultCD = "2004"; 
					//        ds.Dispose();
					//        // ����Ʈ���̽���  Close�Ѵ�
					//        _db.Close();
					//        throw new FrameException("�ش� ��ü������ ���� �α��� ������ �����ϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�.");										
					//}	
					//ds.Dispose();
				//}
				#endregion

				#region �̵� ������ ���Ŀ� ó���Ѵ�.
				// ������� ������ �̵� ������ ���
				//if(loginModel.UserLevel.Equals("30"))
				//{				
				//    //MediaRap ���̺��� ���ڵ��� ��뿩�θ� �˾ƿ��� ����
				//    //1. User���̺� �α��� ID�� �������� ������ �ִ� ����ڰ� �α����� �Ѵ�.
				//    //2. MediaRap���̺��� ���ڵ��� ��뿩�θ� Ȯ�� ��뿩�ΰ� 'N'�̸�
				//    //3.�������� �������ִ� ����ڴ� �α��� �� �� ����.
				//    //4.�α��� �ҷ��� ���� ������ ������ �ִ� ����� MediaRap���̺��� �ش� ���ڵ��� ��뿩�θ� 'Y'�� ���� ����� �Ѵ�.
				//    sbQuery    = new StringBuilder();
				//    sbQuery.Append("\n"
				//        + " SELECT *               \n"								
				//        + "   FROM MediaRap        \n"					
				//        + "  WHERE UseYn = 'Y'     \n"
				//        + "    AND RapCode = '" + loginModel.RapCode   + "'  \n"
				//        );

				//    //__DEBUG
				//    _log.Debug(sbQuery.ToString());
				//    //__DEBUG

				//    ds = new DataSet();
				//    _db.ExecuteQuery(ds, sbQuery.ToString());

				//    //�̵� ����Ʈ�� �Ǽ��� 0���� ũ�� ���� 
				//    if(Utility.GetDatasetCount(ds) == 0)
				//    {
				//            loginModel.ResultCD = "2006"; 
				//            ds.Dispose();
				//            // ����Ʈ���̽���  Close�Ѵ�
				//            _db.Close();
				//        throw new FrameException("�ش� �̵� ������ ���� �α��� ������ �����ϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�.");										
				//    }

				//    ds.Dispose();
				//}
				#endregion

				#region ����� ������ ���Ŀ� ó������
				// ������� ������ ����� ������ ���
				//if(loginModel.UserLevel.Equals("40"))
				//{
				//    //Agency ���̺��� ������ڵ��� ��뿩�θ� �˾ƿ��� ����
				//    //1. User���̺� �α��� ID�� ����� ������ ������ �ִ� ����ڰ� �α����� �Ѵ�.
				//    //2. Agency���̺��� ������ڵ��� ��뿩�θ� Ȯ�� ��뿩�ΰ� 'N'�̸�
				//    //3.����緹���� �������ִ� ����ڴ� �α��� �� �� ����.
				//    //4.�α��� �ҷ��� ���� ������ ������ �ִ� ����� Agency���̺��� �ش� ������ڵ��� ��뿩�θ� 'Y'�� ���� ����� �Ѵ�.
				//    sbQuery    = new StringBuilder();
				//    sbQuery.Append("\n"
				//        + " SELECT *              \n"								
				//        + "   FROM Agency         \n"
				//        + "  WHERE UseYn = 'Y'    \n"
				//        + "    AND AgencyCode = '" + loginModel.AgencyCode + "'  \n"
				//        );

				//    //__DEBUG
				//    _log.Debug(sbQuery.ToString());
				//    //__DEBUG

				//    ds = new DataSet();
				//    _db.ExecuteQuery(ds, sbQuery.ToString());

				//    //Agency ����Ʈ�� �Ǽ��� 0���� ũ�� ���� 
				//    if(Utility.GetDatasetCount(ds) == 0)
				//    {
				//            loginModel.ResultCD = "2007"; 
				//            ds.Dispose();
				//            // ����Ʈ���̽���  Close�Ѵ�
				//            _db.Close();
				//        throw new FrameException("�ش� ����緹���� ���� �α��� ������ �����ϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�.");										
				//    }

				//    ds.Dispose();
				//}
				#endregion

				_log.Debug("<�������>");
				_log.Debug("UserName   :[" + loginModel.UserName   + "]");
				_log.Debug("UserLevel  :[" + loginModel.UserLevel  + "]");
				_log.Debug("UserClass  :[" + loginModel.UserClass  + "]");
				_log.Debug("MediaCode  :[" + loginModel.MediaCode  + "]");
				_log.Debug("MediaName  :[" + loginModel.MediaName  + "]");
				_log.Debug("RepCode    :[" + loginModel.RapCode    + "]");
				_log.Debug("RepName    :[" + loginModel.RapName    + "]");
				_log.Debug("AgencyCode :[" + loginModel.AgencyCode + "]");
				_log.Debug("AgencyName :[" + loginModel.AgencyName + "]");
				_log.Debug("LoginTime  :[" + loginModel.LoginTime  + "]");
					
				loginModel.ResultCD    = "0000"; // ������ȸ
				ds.Dispose();
			}
			catch(FrameException fe)
			{
				loginModel.ResultDesc = fe.ResultMsg;
				_db.Close();
				return;
			}
			catch(Exception ex)
			{
				loginModel.ResultCD = "2005";  // �α��ο���
				loginModel.ResultDesc = "�α��ε��� ������ �߻��Ͽ����ϴ�\n" + ex.Message;
				_log.Error(this.ToString() + ":" + loginModel.ResultDesc);
				_db.Close();
				return;
			}

			// ����� ���̺� ������� �ֱٷα��νð��� ������Ʈ �Ѵ�.
			try 
			{
				StringBuilder sbQuery    = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				_db.BeginTran();	// Ʈ������� ����

				sbQuery = new StringBuilder();
				sbQuery.Append("\n UPDATE STM_USER SET LAST_LOGIN = SYSDATE WHERE USER_ID = :UserID");
				
				sqlParams[0] = new OracleParameter(":UserID" , OracleDbType.Varchar2 , 10);
				sqlParams[0].Value = loginModel.UserID;

				int rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
				_db.CommitTran();
			}
			catch(Exception ex)
			{
				loginModel.ResultCD = "2005";
				_db.RollbackTran();
				loginModel.ResultDesc = "�α��ε��� ������ �߻��Ͽ����ϴ�\n" + ex.Message;
				_log.Error(this.ToString() + ":" + loginModel.ResultDesc);

				_db.Close();
				return;
			}
			
			// __MESSAGE__
			_log.Message("[" + header.ClientKey + "] " + loginModel.UserID + "(" + loginModel.UserName + ")�� �α���");
		
			_db.Close();
			_log.Debug("-----------------------------------------");
			_log.Debug(this.ToString() + ":LoginCheck() End");
			_log.Debug("-----------------------------------------");
		}
	}
}