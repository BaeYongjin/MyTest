// ===============================================================================
//
// LoginBiz.cs
//
// 로그인 처리로직 
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
	/// LoginService에 대한 요약 설명입니다.
	/// </summary>
    public class LoginBiz : BaseBiz
	{
		public LoginBiz() : base(FrameSystem.connDbString, true)
		{
		    _log    = FrameSystem.oLog;			// 로그 객체
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
		/// 로그인 처리
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
  
				_log.Debug("<입력정보>");
				_log.Debug("UserID       :[" + loginModel.UserID       + "]");
				_log.Debug("UserPassword :[" + loginModel.UserPassword + "]");
				_log.Debug("SystemVersion:[" + loginModel.SystemVersion + "]");
				
				if(FrameSystem.m_SystemVersion == null || FrameSystem.m_SystemVersion.Equals("") || !FrameSystem.m_SystemVersion.Equals(loginModel.SystemVersion))
				{
					loginModel.ResultCD = "2000";	

					throw new FrameException("프로그램버전이 서버와 일치하지 않습니다.\n프로그램을 업데이트하시기 바랍니다.\n\n서버버전:" + FrameSystem.m_SystemVersion);
				}

				if(loginModel.UserID.Trim().Length == 0)
				{
					loginModel.ResultCD = "2001"; // 사용자ID가 입력되지 않았음.

					throw new FrameException("사용자ID가 입력되지 않았습니다.");
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
					loginModel.ResultCD = "2002"; // 해당 ID가 DB에 존재하지 않음
					ds.Dispose();
					_db.Close();
					throw new FrameException("해당ID가 존재하지 않습니다.");
				}

                //비밀번호 알아내기
                //_log.Debug("PW:"+Utility.GetDatasetString(ds, 0, "UserPassword") +">"+Security.Decrypt(Utility.GetDatasetString(ds, 0, "UserPassword")));
				// 암호화된 비밀번호가 다르면 비밀번호 오류
				if (!Utility.GetDatasetString(ds, 0, "UserPassword").Equals(loginModel.UserPassword))
				{
					loginModel.ResultCD = "2003";  // 비밀번호가 다름
					ds.Dispose();
					_db.Close();
					throw new FrameException("비밀번호가 일치하지 않습니다.");
				}				
						
				// 로그인정보셋트
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

				#region 매체레벨은 딱히 검증할것이 없다
				// 사용자의 레벨이 매체레벨일 경우
				//if(loginModel.UserLevel.Equals("20"))
				//{
					//Media 테이블에서 매체코드의 사용여부를 알아오기 위해
					//1. User테이블 로그인 ID가 매체레벨을 가지고 있는 사용자가 로그인을 한다.
					//2. Media테이블의 매체코드의 사용여부를 확인 사용여부가 'N'이면
					//3.매체레벨을 가지고있는 사용자는 로그인 할 수 없다.
					//4.로그인 할려면 어드민 권한을 가지고 있는 사람이 Media테이블의 해당 매체코드의 사용여부를 'Y'로 변경 해줘야 한다.
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

					////매체 리스트의 건수가 0보다 크면 실행 
					//if(Utility.GetDatasetCount(ds) == 0)
					//{
					//        loginModel.ResultCD = "2004"; 
					//        ds.Dispose();
					//        // 데이트베이스를  Close한다
					//        _db.Close();
					//        throw new FrameException("해당 매체레벨에 대한 로그인 권한이 없습니다.\n관리자에게 문의하시기 바랍니다.");										
					//}	
					//ds.Dispose();
				//}
				#endregion

				#region 미디어렙 레벨은 추후에 처리한다.
				// 사용자의 레벨이 미디어렙 레벨일 경우
				//if(loginModel.UserLevel.Equals("30"))
				//{				
				//    //MediaRap 테이블에서 랩코드의 사용여부를 알아오기 위해
				//    //1. User테이블 로그인 ID가 랩레벨을 가지고 있는 사용자가 로그인을 한다.
				//    //2. MediaRap테이블의 랩코드의 사용여부를 확인 사용여부가 'N'이면
				//    //3.랩레벨을 가지고있는 사용자는 로그인 할 수 없다.
				//    //4.로그인 할려면 어드민 권한을 가지고 있는 사람이 MediaRap테이블의 해당 랩코드의 사용여부를 'Y'로 변경 해줘야 한다.
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

				//    //미디어렙 리스트의 건수가 0보다 크면 실행 
				//    if(Utility.GetDatasetCount(ds) == 0)
				//    {
				//            loginModel.ResultCD = "2006"; 
				//            ds.Dispose();
				//            // 데이트베이스를  Close한다
				//            _db.Close();
				//        throw new FrameException("해당 미디어렙 레벨에 대한 로그인 권한이 없습니다.\n관리자에게 문의하시기 바랍니다.");										
				//    }

				//    ds.Dispose();
				//}
				#endregion

				#region 대행사 레벨도 추후에 처리하자
				// 사용자의 레벨이 대행사 레벨일 경우
				//if(loginModel.UserLevel.Equals("40"))
				//{
				//    //Agency 테이블에서 대행사코드의 사용여부를 알아오기 위해
				//    //1. User테이블 로그인 ID가 대행사 레벨을 가지고 있는 사용자가 로그인을 한다.
				//    //2. Agency테이블의 대행사코드의 사용여부를 확인 사용여부가 'N'이면
				//    //3.대행사레벨을 가지고있는 사용자는 로그인 할 수 없다.
				//    //4.로그인 할려면 어드민 권한을 가지고 있는 사람이 Agency테이블의 해당 대행사코드의 사용여부를 'Y'로 변경 해줘야 한다.
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

				//    //Agency 리스트의 건수가 0보다 크면 실행 
				//    if(Utility.GetDatasetCount(ds) == 0)
				//    {
				//            loginModel.ResultCD = "2007"; 
				//            ds.Dispose();
				//            // 데이트베이스를  Close한다
				//            _db.Close();
				//        throw new FrameException("해당 대행사레벨에 대한 로그인 권한이 없습니다.\n관리자에게 문의하시기 바랍니다.");										
				//    }

				//    ds.Dispose();
				//}
				#endregion

				_log.Debug("<출력정보>");
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
					
				loginModel.ResultCD    = "0000"; // 정상조회
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
				loginModel.ResultCD = "2005";  // 로그인오류
				loginModel.ResultDesc = "로그인도중 오류가 발생하였습니다\n" + ex.Message;
				_log.Error(this.ToString() + ":" + loginModel.ResultDesc);
				_db.Close();
				return;
			}

			// 사용자 테이블에 사용자의 최근로그인시각을 업데이트 한다.
			try 
			{
				StringBuilder sbQuery    = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				_db.BeginTran();	// 트랜잭션의 시작

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
				loginModel.ResultDesc = "로그인도중 오류가 발생하였습니다\n" + ex.Message;
				_log.Error(this.ToString() + ":" + loginModel.ResultDesc);

				_db.Close();
				return;
			}
			
			// __MESSAGE__
			_log.Message("[" + header.ClientKey + "] " + loginModel.UserID + "(" + loginModel.UserName + ")님 로그인");
		
			_db.Close();
			_log.Debug("-----------------------------------------");
			_log.Debug(this.ToString() + ":LoginCheck() End");
			_log.Debug("-----------------------------------------");
		}
	}
}